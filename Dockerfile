# Backend
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS backend-build

WORKDIR /app

# Copy the rest of the source code and build the application
COPY ./API ./backend

WORKDIR /app/backend

RUN dotnet restore
RUN dotnet build -c Release --no-restore

# Publish the application
RUN dotnet publish -c Release --no-restore --no-build -o /app/backend/publish


# Backend runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS backend-runtime
WORKDIR /app
COPY --from=backend-build /app/backend/publish .

EXPOSE 8080

# Set the entry point
ENTRYPOINT ["dotnet", "API.dll"]


# App
FROM node:lts as app-build

WORKDIR /app

COPY ./app ./frontend

WORKDIR /app/frontend

RUN npm install -g bun

RUN bun install

RUN bun run build

FROM nginx:stable-alpine AS app-runtime

COPY --from=app-build /app/frontend/build /usr/share/nginx/html

# This is not great, but it's PROBABLY fine for now on a non-public site.
COPY /app/.env /usr/share/nginx/html

COPY app/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]