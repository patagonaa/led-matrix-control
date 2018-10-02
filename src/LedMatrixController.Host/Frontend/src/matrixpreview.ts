import * as signalR from "@aspnet/signalr";

class MatrixPreview {
    private canvasContext: CanvasRenderingContext2D;
    private backgroundCanvasContext: CanvasRenderingContext2D;
    private width: number;
    private height: number;
    
    constructor(canvasElement: HTMLCanvasElement, width: number, height: number, scale: number) {
        this.canvasContext = canvasElement.getContext("2d");
        canvasElement.width  = width*scale;
        canvasElement.height = height*scale;
        this.canvasContext.scale(scale, scale);
        this.canvasContext.imageSmoothingEnabled = false;

        var backgroundCanvasElement = document.createElement("canvas");
        backgroundCanvasElement.width = width;
        backgroundCanvasElement.height = height;
        this.backgroundCanvasContext = backgroundCanvasElement.getContext("2d");
        
        this.width = width;
        this.height = height;
    }

    base64ToUint8ClampedArray(base64: string){
        var raw = window.atob(base64);
        var rawLength = raw.length;
        var array = new Uint8ClampedArray(new ArrayBuffer(rawLength));
      
        for(var i = 0; i < rawLength; i++) {
          array[i] = raw.charCodeAt(i);
        }
        return array;
    }

    async startListen() {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("/matrixpreview")
            .configureLogging(signalR.LogLevel.Information)
            .build();

        connection.on("PreviewFrame", (frame: {imageData: string}) => {
            var imageData = new ImageData(this.base64ToUint8ClampedArray(frame.imageData), this.width, this.height);
            this.backgroundCanvasContext.putImageData(imageData, 0, 0);
            this.canvasContext.drawImage(this.backgroundCanvasContext.canvas, 0, 0);
        });

        await connection.start();
    }
}