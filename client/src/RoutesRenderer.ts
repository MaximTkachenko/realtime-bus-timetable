import * as SVG from "@svgdotjs/svg.js";
import {Root} from './Metadata';

export default class RoutesRenderer{
    busStopRadius: number;
    busStopOffset: number;
    busRadius: number;
    busOffset: number;
    server: string;
    metadata: Root;
    buses: SVG.Circle[];
    draw: SVG.Svg;

    constructor(metadata: Root, server: string){
        this.busStopRadius = 36;
        this.busStopOffset = this.busStopRadius / 2;
        this.busRadius = 20;
        this.busOffset = this.busRadius / 2;
        this.server = server;
        this.metadata = metadata;
        this.buses = [];

        this.draw = SVG.SVG().addTo('#routes').size(this.metadata.width, this.metadata.height);

        this.renderRoutes();
        this.renderBusStops();
        this.renderBuses();  
        this.startRoutes();
        this.trackRoutes();
    }

    private renderRoutes(){
        for(let i = 0; i < this.metadata.routes.length; i++) {
            const route = this.metadata.routes[i];

           let arr = '';
           for(let j = 0; j < route.path.length; j++) {
               arr += `${this.metadata.busStops[route.path[j].busStopIndex].x},${this.metadata.busStops[route.path[j].busStopIndex].y}`;
               if(j < route.path.length - 1){
                   arr += ' ';
               }               
           }
           
           this.draw.polyline(arr)
               .fill('none')
               .stroke({ color: route.color, width: 4, linecap: 'round', linejoin: 'round' })
       } 
    }

    private renderBuses(){
        for(let i = 0; i < this.metadata.routes.length; i++) {
            const route = this.metadata.routes[i];
               
            const firstBusStop = this.metadata.busStops[route.path[0].busStopIndex];
            const routeCircle = this.draw.circle(this.busRadius)
                .x(firstBusStop.x - this.busOffset)
                .y(firstBusStop.y - this.busOffset)
                .fill(this.metadata.routes[i].color)
                .stroke({ color: 'white', width: 2})
                .attr('id', route.id)
                .attr('index', i)
                .attr('direction', true);
            this.buses.push(routeCircle);
       } 
    }

    private renderBusStops(){
        for(let i = 0; i < this.metadata.busStops.length; i++) {
            const stop = this.metadata.busStops[i];
            this.draw.circle(this.busStopRadius).x(stop.x - this.busStopOffset).y(stop.y - this.busStopOffset)
                .stroke({ color: 'black', width: 4}).fill(this.metadata.busStopColor)
                .add(this.draw.element('title').words(stop.id))
                .attr('id', stop.id)
                .click((e: any) => {
                    let busStopSelected = new CustomEvent('busStopSelected', {'detail': e.target.id});
                    document.dispatchEvent(busStopSelected);
                });
        }
    }

    private startRoutes() {            
        for(let i = 0; i < this.metadata.routes.length; i++) {
            this.startRoute(this.buses[i]);
        }
    }

    private startRoute(bus: SVG.Circle){
        const routeIndex = bus.attr('index');        
        const direction = bus.attr('direction') === 'true';
        bus.attr('direction', !direction);
        const route = this.metadata.routes[routeIndex];

        const routeBuilder = (i: number, isLast: boolean, direction: boolean) => {
            const nextStop = this.metadata.busStops[route.path[i].busStopIndex];
            const duration = direction ? route.path[i].duration : route.path[i + 1].duration;
            const animateConfig = { ease: '--', duration: duration, delay: this.metadata.timeSpentOnBusStopMs };
            const routeRunner = bus.animate(animateConfig).move(nextStop.x - this.busOffset, nextStop.y - this.busOffset);
            if(isLast){
                routeRunner.after((e: any) => {this.startRoute(e.detail._element);});
            }
        };
        
        if(direction){
            for(let j = 1; j < route.path.length; j++) {
                routeBuilder(j, j === route.path.length - 1, direction);
            }
        } else{            
            for(let j = route.path.length - 2; j > -1; j--) {
                routeBuilder(j, j === 0, direction);
            }
        }
    }

    private trackRoutes(){
        setInterval(async () => {
            const promises: Promise<Response>[] = [];
            for(let i = 0; i < this.buses.length; i++) {
                const promise = fetch(this.server + '/' + this.buses[i].attr('id') + '/location', {
                    method: 'POST',
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({x: this.buses[i].node.cx.baseVal.value, y: this.buses[i].node.cy.baseVal.value, unixTimestamp: Date.now() / 1000})
                });

                promises.push(promise);
            }    
            
            await Promise.all(promises);
        }, this.metadata.trackRoutesIntervalMs);
    }
}