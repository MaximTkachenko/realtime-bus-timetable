![.NET Core](https://github.com/MaximTkachenko/realtime-bus-timetable/workflows/.NET%20Core/badge.svg)

# realtime-bus-timetable [WORK IN PROGRESS]

## Sample

![](sample.gif)

## How to run it locally:
- run orleans backend:
```bash
cd src/BusTimetable
dotnet run
```
- backend is hosted on `5005` port
- orleans dashboard is hosted on `5006` port 
- run frontend:
```
cd /client
npm run start
```
- type `http://localhost:5005` in host input and click `Go` button
- click on any circle on the map to start tracking

## Stack:
- frontend: [typescript](https://www.typescriptlang.org/), [react](https://create-react-app.dev/docs/getting-started/), [svg.js](https://svgjs.com)
- backend: [asp.net core](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-3.1), [orleans](https://dotnet.github.io/orleans/)

Useful links:
- https://svgjs.com/docs/3.0/
- https://dotnet.github.io/orleans/Documentation/index.html
- https://github.com/sketch7/orleans-heroes

## ToDo:
- [DONE] basic logic - [v1](https://github.com/MaximTkachenko/realtime-bus-timetable/releases/tag/v1)
- host orleans cluster on heroku?
- [orleans meetups](https://github.com/OrleansContrib/meetups)
- smarttimetable + benhmarks + remove route on arrival
- clustering (sql server) + local docker + haproxy
- loadtests + singleton service
- aci + traffic manager + clustering (table storage) = https://aaronmsft.com/posts/azure-container-instances/
- persistence (table storage)
- put into AKS
- finished generator


For BusTimetable.Generator: https://stackoverflow.com/questions/11178414/algorithm-to-generate-equally-distributed-points-in-a-polygon
