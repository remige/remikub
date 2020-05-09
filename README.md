# remikub

WIP

Build the WebApi image : 
-  `docker build -t remikub .`

Run as standalone :
- `docker run -d -p 8080:80 --name remikub remikub`

Run with al dependencies :
- `docker-compose up`

Push to registry :
- Cf Action


Deploy on server
- `docker login docker.pkg.github.com -u remige`
- `docker pull docker.pkg.github.com/remige/remikub/remikub:latest`
- `docker run -d -p 80:80 docker.pkg.github.com/remige/remikub/remikub:latest --name  remikub`