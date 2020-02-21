import * as SVG from "@svgdotjs/svg.js";
import {Root} from './Metadata';

export default class RouesRendere{
    init(){
        document.addEventListener("routesReady", (e: any) => {
            let data: Root = e.detail;
            console.log(data);

            const draw = SVG.SVG().addTo('#routes').size(data.width, data.height);

            for(let i = 0; i < data.routes.length; i++) {
                 const route = data.routes[i];

                let arr = '';
                for(let j = 0; j < route.path.length; j++) {
                    arr += `${route.path[j].x},${route.path[j].y}`;
                    if(j < route.path.length - 1){
                        arr += ' ';
                    }
                    
                }
                var routePolyline = draw.polyline(arr)
                    .fill('none')
                    .stroke({ color: route.color, width: 1, linecap: 'round', linejoin: 'round' })
            }

            
            
            for(let i = 0; i < data.busStops.length; i++) {
                const stop = data.busStops[i];
                const stopCircle = draw.circle(20).x(stop.x - 10).y(stop.y - 10).fill(stop.color);
            }

            /*
            const rect = draw.circle(20).x(10).y(10).fill('#f06');
            const animateConfig = { ease: '<>', duration: 1000, delay: 0 };
            rect.animate(animateConfig).move(200, 200);
            rect.animate(animateConfig).move(200, 400);
            rect.animate(animateConfig).move(600, 400);
            rect.animate(animateConfig).move(10, 10);
            let i = 0;
            setInterval(function() { if(i > 50) return;console.log(rect.node.cx.baseVal.value); i++;}, 100);
                    });*/

        });
    }
}