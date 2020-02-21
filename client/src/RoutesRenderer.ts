import * as SVG from "@svgdotjs/svg.js";
import {Root} from './Metadata';

export default class RouesRendere{
    init(){
        document.addEventListener("routesReady", (e: any)=>{
            let data: Root = e.detail;
            console.log(data);

            const draw = SVG.SVG().addTo('#routes').size('900px', '900px');
            
            const rect = draw.circle(20).x(10).y(10).fill('#f06');
            const animateConfig = { ease: '<>', duration: 1000, delay: 0 };
            rect.animate(animateConfig).move(200, 200);
            rect.animate(animateConfig).move(200, 400);
            rect.animate(animateConfig).move(600, 400);
            rect.animate(animateConfig).move(10, 10);
            let i = 0;
            setInterval(function() { if(i > 50) return;console.log(rect.node.cx.baseVal.value); i++;}, 100);
                    });
    }
}