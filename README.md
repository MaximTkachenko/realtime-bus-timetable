# realtime-bus-timetable
Real time bus timetable

Stack:
- frontend: typescript, react, [svg.js](https://svgjs.com)
- backend: asp.net core, [orleans](https://dotnet.github.io/orleans/)
- signalr: pushing timetable from server instead of pulling from clients

Useful links:
- https://svgjs.com/docs/2.7/
- https://dotnet.github.io/orleans/Documentation/index.html
- https://dev.to/dirk94/how-i-structure-my-express-typescript-react-applications-g3e

Tasks:
- articles for blog
- talk for STVG developer group

ToDo:
- [DONE] basic logic - [v1](https://github.com/MaximTkachenko/realtime-bus-timetable/releases/tag/v1)
- smarttimetable + benhmarks
- clustering (sql server) + local docker + haproxy
- loadtests + singleton service
- aci + traffic manager + clustering (table storage) = https://aaronmsft.com/posts/azure-container-instances/
- persistence (table storage)
- put into AKS
- finished generator


For BusTimetable.Generator: https://stackoverflow.com/questions/11178414/algorithm-to-generate-equally-distributed-points-in-a-polygon
