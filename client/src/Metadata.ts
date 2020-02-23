export type Root = {    
    width: number,
    height: number,
    routes: Route[],
    busStops: BusStop[]
}

export type Route = {
    id: string,
    color: string,
    path: number[]
}

export type BusStop = {
    id: string,
    color: string,
    x: number,
    y: number
}