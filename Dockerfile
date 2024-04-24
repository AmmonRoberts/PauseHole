# Stage 1: Build the React application
FROM node:lts as build

WORKDIR /app

COPY package*.json ./

RUN npm install -g bun

RUN bun install

COPY . .

RUN bun run build

# Stage 2: Serve the app using Nginx
FROM nginx:stable-alpine

COPY --from=build /app/build /usr/share/nginx/html

# Copy the default nginx.conf provided by tiangolo/node-frontend
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]