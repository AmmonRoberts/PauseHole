FROM node:lts as build

WORKDIR /app

COPY package*.json ./

RUN npm install -g bun

RUN bun install

COPY . .

RUN bun run build

FROM nginx:stable-alpine

COPY --from=build /app/build /usr/share/nginx/html

# This is not great, but it's PROBABLY fine for now on a non-public site.
COPY .env /usr/share/nginx/html

COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]