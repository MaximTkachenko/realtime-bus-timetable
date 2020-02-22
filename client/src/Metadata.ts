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
    x: number,
    y: number,
    busStopId: string,
    isBusStop: boolean
}

export type BusStop = {
    id: string,
    color: string,
    x: number,
    y: number
}