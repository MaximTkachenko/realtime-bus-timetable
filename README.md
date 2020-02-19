# realtime-bus-timetable
Real time bus timetable

Stack:
- frontend: typescript, react, [svg.js](https://svgjs.com)
- backend: asp.net core, [orleans](https://dotnet.github.io/orleans/)
- signalr, azure signalr for AKS hosting

Useful links:
- https://svgjs.com/docs/2.7/
- https://dotnet.github.io/orleans/Documentation/index.html
- https://dev.to/dirk94/how-i-structure-my-express-typescript-react-applications-g3e

Tasks:
- article for blog
- talk for STVG developer group

ToDo:
- need route generator (100 routes)
- basic logic
- persistence
- clustering
- dockerize, put into AKS

```json
{
	routes: [
		{ 
			"path": [
				{ "x": 0, "y": 0, "isBusStop": true, "name": "", "predictedTravelTime": 5 },
				{ "x": 0, "y": 0, "isBusStop": false }
			],
			"name": "",
			"color": "",			
		}
	],
	busStops: [
		{ "x": 0, "y": 0, "name": "" }
	]
}
```

```html
<div id="drawing"></div>
```
```js
// initialize SVG.js
var draw = SVG('drawing')

// draw pink square
const rect = draw.circle(20).move(10, 10).fill('#f06');
rect.animate(1000, '-', 0).move(100, 100);
rect.animate(1000, '-', 0).move(300, 100);
rect.animate(1000, '-', 0).move(100, 100);
let i = 0;
setInterval(function() { if(i > 50) return;console.log(rect.node.cx.baseVal.value); i++;}, 100);
```
