export class Root {    
    width: number;
    height: number;
    routes: Route[];
    busStops: BusStop[];
    
    constructor() {
        this.routes = [];
        this.busStops = [];
        this.width = 0;
        this.height = 0;
    }
}

export class Route {
    id: string;
    color: string;
    path: Point[];
    
    constructor() {
        this.id = '';
        this.color = '';
        this.path = [];
    }
}

export class Point{
    x: number;
    y: number;
    busStopId: string;
    isBusStop: boolean;
    
    constructor() {
        this.x = 0;
        this.y = 0;
        this.busStopId = '';
        this.isBusStop = false;
    }
}

export class BusStop {
    id: string;
    color: string;
    x: number;
    y: number;
    
    constructor() {
        this.id = '';
        this.color = '';
        this.x = 0;
        this.y = 0;
    }
}