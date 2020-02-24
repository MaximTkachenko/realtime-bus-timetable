import * as SVG from "@svgdotjs/svg.js";
import {Root} from './Metadata';

export default class RoutesRenderer{
    busStopRadius: number;
    busStopOffset: number;
    busRadius: number;
    busOffset: number;
    timeSpentOnBusStop: number;
    trackRouesIntervalMs: number;

    constructor(){
        this.busStopRadius = 36;
        this.busStopOffset = this.busStopRadius / 2;
        this.busRadius = 20;
        this.busOffset = this.busRadius / 2;
        this.timeSpentOnBusStop = 500;
        this.trackRouesIntervalMs = 300;
    }

    init(){
        document.addEventListener("routesReady", (e: any) => {
            const data: Root = e.detail.metadata;
            const server: string = e.detail.server;

            const draw = SVG.SVG().addTo('#routes').size(data.width, data.height);

            this.renderRoutes(draw, data);
            this.renderBusStops(draw, data);
            const buses: SVG.Circle[] = this.startRoutes(draw, data);
            this.trackRoutes(buses, server);
        });
    }

    renderRoutes(draw: SVG.Svg, data: Root){
        for(let i = 0; i < data.routes.length; i++) {
            const route = data.routes[i];

           let arr = '';
           for(let j = 0; j < route.path.length; j++) {
               arr += `${data.busStops[route.path[j].busStopIndex].x},${data.busStops[route.path[j].busStopIndex].y}`;
               if(j < route.path.length - 1){
                   arr += ' ';
               }               
           }
           
           draw.polyline(arr)
               .fill('none')
               .stroke({ color: route.color, width: 4, linecap: 'round', linejoin: 'round' })
       }   
    }

    renderBusStops(draw: SVG.Svg, data: Root){
        for(let i = 0; i < data.busStops.length; i++) {
            const stop = data.busStops[i];
            draw.circle(this.busStopRadius).x(stop.x - this.busStopOffset).y(stop.y - this.busStopOffset)
                .stroke({ color: 'black', width: 4}).fill(stop.color)
                .add(draw.element('title').words(stop.id))
                .attr('id', stop.id)
                .click((e: any) => {
                    let busStopSelected = new CustomEvent('busStopSelected', {'detail': e.target.id});
                    document.dispatchEvent(busStopSelected);
                });
        }
    }

    startRoutes(draw: SVG.Svg, data: Root) {
        const buses: SVG.Circle[] = [];
            
        for(let i = 0; i < data.routes.length; i++) {
            const route = data.routes[i];
            const firstBusStop = data.busStops[route.path[0].busStopIndex];

            const routeCircle = draw.circle(this.busRadius).x(firstBusStop.x - this.busOffset).y(firstBusStop.y - this.busOffset)
                .fill(data.routes[i].color).stroke({ color: 'white', width: 2}).attr('id', route.id);
            buses.push(routeCircle);

            for(let j = 1; j < route.path.length; j++) {
                const nextStop = data.busStops[route.path[j].busStopIndex];
                const animateConfig = { ease: '--', duration: route.path[j].duration, delay: this.timeSpentOnBusStop };
                routeCircle.animate(animateConfig).move(nextStop.x - this.busOffset, nextStop.y - this.busOffset);
            }
        }
        return buses;
    }

    trackRoutes(buses: SVG.Circle[], server: string){
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
        }, this.trackRouesIntervalMs);
    }
}