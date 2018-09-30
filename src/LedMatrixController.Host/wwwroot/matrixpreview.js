class MatrixPreview {
    constructor(canvasElement, width, height, scale) {
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
        this.scale = scale;
    }

    base64ToUint8ClampedArray(base64){
        var raw = window.atob(base64);
        var rawLength = raw.length;
        var array = new Uint8ClampedArray(new ArrayBuffer(rawLength));
      
        for(var i = 0; i < rawLength; i++) {
          array[i] = raw.charCodeAt(i);
        }
        return array;
    }

    startListen() {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("/matrixpreview")
            .configureLogging(signalR.LogLevel.Information)
            .build();

        connection.on("PreviewFrame", (frame) => {
            var imageData = new ImageData(this.base64ToUint8ClampedArray(frame.imageData), this.width, this.height);
            this.backgroundCanvasContext.putImageData(imageData, 0, 0);
            this.canvasContext.drawImage(this.backgroundCanvasContext.canvas, 0, 0);
        });

        connection.start().catch(err => console.error(err.toString()));
    }
}