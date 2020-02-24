export type Root = {    
    width: number,
    height: number,
    routes: Route[],
    busStops: BusStop[]
}

export type Route = {
    id: string,
    color: string,
    path: Point[]
}

export type Point = {
    busStopIndex: number,
    duration: number
}

export type BusStop = {
    id: string,
    color: string,
    x: number,
    y: number
}