import * as SVG from "@svgdotjs/svg.js";
import {Root} from './Metadata';

export default class RouesRendere{
    init(){
        document.addEventListener("routesReady", (e: any) => {
            let data: Root = e.detail.metadata;
            let server: string = e.detail.server;
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
                draw.polyline(arr)
                    .fill('none')
                    .stroke({ color: route.color, width: 1, linecap: 'round', linejoin: 'round' })
            }

            
            
            for(let i = 0; i < data.busStops.length; i++) {
                const stop = data.busStops[i];
                const stopCircle = draw.circle(20).x(stop.x - 10).y(stop.y - 10).fill(stop.color);
            }
            
            const buses: SVG.Circle[] = [];
            const animateConfig = { ease: '<>', duration: 4000, delay: 0 };
            for(let i = 0; i < data.routes.length; i++) {
                const route = data.routes[i];

                const rect = draw.circle(40).x(route.path[0].x - 20).y(route.path[0].y - 20).fill(data.routes[i].color).attr('id', route.id);
                buses.push(rect);

                for(let j = 1; j < route.path.length; j++){
                    rect.animate(animateConfig).move(route.path[j].x - 20, route.path[j].y - 20);
                }
           }

           setInterval(async () => { 

                const promises: Promise<Response>[] = [];
                for(let i = 0; i < buses.length; i++) {
                    const promise = fetch(server + '/' + buses[i].attr('id') + '/location/', {
                        method: 'POST',
                        headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                        },
                        body: JSON.stringify({x: buses[i].node.cx.baseVal.value, y: buses[i].node.cy.baseVal.value})
                    });

                    promises.push(promise);
                }    
                
                await Promise.all(promises);
            }, 200);

        });
    }
}