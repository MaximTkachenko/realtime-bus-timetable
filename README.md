![.NET Core](https://github.com/MaximTkachenko/realtime-bus-timetable/workflows/.NET%20Core/badge.svg)

# realtime-bus-timetable [WORK IN PROGRESS]

How to run it locally:
- run orleans backend:
```
cd src\BusTimetable
dotnet run
```
- backend listens on port 5005
- run frontend:
```
cd \client
npm start
```
- type `http://localhost:5005` in host input and click `GO`
- click `Start` and then click on any circle on the map

Stack:
- frontend: typescript, [react](https://create-react-app.dev/docs/getting-started/), [svg.js](https://svgjs.com)
- backend: asp.net core, [orleans](https://dotnet.github.io/orleans/)

Useful links:
- https://svgjs.com/docs/2.7/
- https://dotnet.github.io/orleans/Documentation/index.html

ToDo:
- [DONE] basic logic - [v1](https://github.com/MaximTkachenko/realtime-bus-timetable/releases/tag/v1)
- [orleans meetups](https://github.com/OrleansContrib/meetups)
- smarttimetable + benhmarks + remove route on arrival
- clustering (sql server) + local docker + haproxy
- loadtests + singleton service
- aci + traffic manager + clustering (table storage) = https://aaronmsft.com/posts/azure-container-instances/
- persistence (table storage)
- put into AKS
- finished generator


For BusTimetable.Generator: https://stackoverflow.com/questions/11178414/algorithm-to-generate-equally-distributed-points-in-a-polygon
