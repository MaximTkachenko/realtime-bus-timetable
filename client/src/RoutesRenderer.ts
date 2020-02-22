import * as SVG from "@svgdotjs/svg.js";
import {Root} from './Metadata';

export default class RoutesRendere{
    init(){
        const busStopRadius: number = 36;
        const busStopOffset: number = busStopRadius / 2;
        const busRadius: number = 20;
        const busOffset: number = busRadius / 2;
        const timeSpentOnBusStop: number = 500;
        
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
                    .stroke({ color: route.color, width: 2, linecap: 'round', linejoin: 'round' })
            }            
            
            for(let i = 0; i < data.busStops.length; i++) {
                const stop = data.busStops[i];
                draw.circle(busStopRadius).x(stop.x - busStopOffset).y(stop.y - busStopOffset)
                    .stroke({ color: 'black', width: 4}).fill(stop.color)
                    .add(draw.element('title').words(stop.id))
                    .attr('id', stop.id)
                    .click((e: any) => {
                        let busStopSelected = new CustomEvent('busStopSelected', {'detail': e.target.id});
                        document.dispatchEvent(busStopSelected);
                    });
            }
            
            const buses: SVG.Circle[] = [];
            
            for(let i = 0; i < data.routes.length; i++) {
                const route = data.routes[i];

                const routeCircle = draw.circle(busRadius).x(route.path[0].x - busOffset).y(route.path[0].y - busOffset)
                    .fill(data.routes[i].color).stroke({ color: 'white', width: 2}).attr('id', route.id);
                buses.push(routeCircle);

                for(let j = 1; j < route.path.length; j++) {
                    const animateConfig = { ease: '--', duration: route.path[j].duration, delay: timeSpentOnBusStop };
                    routeCircle.animate(animateConfig).move(route.path[j].x - busOffset, route.path[j].y - busOffset);
                }
           }

           setInterval(async () => {
                const promises: Promise<Response>[] = [];
                for(let i = 0; i < buses.length; i++) {
                    const promise = fetch(server + '/' + buses[i].attr('id') + '/location', {
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