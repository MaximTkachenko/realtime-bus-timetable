export class Root {
    routes: Route[];
    busStops: BusStop[];
    
    constructor() {
        this.routes = [];
        this.busStops = [];
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
        this.x = -1;
        this.y = -1;
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
        this.x = -1;
        this.y = -1;
    }
}