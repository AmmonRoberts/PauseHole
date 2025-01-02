# App
FROM node:lts as app-build

WORKDIR /app

COPY ./ ./frontend

WORKDIR /app/frontend

RUN npm install -g bun

RUN bun install

RUN bun run build

FROM nginx:stable-alpine AS app-runtime

COPY --from=app-build /app/frontend/build /usr/share/nginx/html

# This is not great, but it's PROBABLY fine for now on a non-public site.
COPY /.env /usr/share/nginx/html

COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]