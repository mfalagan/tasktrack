FROM node:latest AS build
WORKDIR /app
COPY . .
RUN npm install -g @angular/cli
RUN npm install
RUN ng build --output-path=dist

FROM nginx:alpine
COPY --from=build /app/dist/* /usr/share/nginx/html
COPY ./nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]