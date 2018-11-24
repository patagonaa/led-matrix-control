import { Guid } from "./uuidv4";

export class ServerConfig {
    id: Guid;
    frameRate: number;
    width: number;
    height: number;
}