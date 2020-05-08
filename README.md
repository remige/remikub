# remikub

Build the WebApi image : 
-  `docker build -t remikub .`

Run as standalone :
- `docker run -d -p 8080:80 --name remikub remikub`

Run with al dependencies :
- `docker-compose up`

Push to docker hub
- `docker login`
- `docker images`
- `docker tag ed8ceafe9149 remige/remikub:0.0.1`
- `docker push remige/remikub`