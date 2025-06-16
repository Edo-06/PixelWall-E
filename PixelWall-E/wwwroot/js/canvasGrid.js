
window.drawGrid = (canvasId, pixelSize, color) => {
    const canvas = document.getElementById(canvasId);
    const ctx = canvas.getContext('2d');
    ctx.imageSmoothingEnabled = false;
    ctx.mozImageSmoothingEnabled = false;
    ctx.webkitImageSmoothingEnabled = false;
    ctx.msImageSmoothingEnabled = false;
    const width = canvas.width;
    const height = canvas.height;

    ctx.strokeStyle = color;
    ctx.lineWidth = 0.2;

    
    for (let x = 0; x <= width; x += pixelSize) {
        ctx.beginPath();
        ctx.moveTo(x, 0);
        ctx.lineTo(x, height);
        ctx.stroke();
    }

    
    for (let y = 0; y <= height; y += pixelSize) {
        ctx.beginPath();
        ctx.moveTo(0, y);
        ctx.lineTo(width, y);
        ctx.stroke();
    }
};

window.clearCanvas = (canvasId, backgroundColor = "#FFFFFF") => {
    const canvas = document.getElementById(canvasId);
    const ctx = canvas.getContext('2d');
    ctx.fillStyle = backgroundColor;
    ctx.fillRect(0, 0, canvas.width, canvas.height);
};

window.fillPixel = (canvasId, pixelSize, x, y, color, gridColor) => {
    const canvas = document.getElementById(canvasId);
    const ctx = canvas.getContext('2d');
    ctx.imageSmoothingEnabled = false;
    ctx.mozImageSmoothingEnabled = false;
    ctx.webkitImageSmoothingEnabled = false;
    ctx.msImageSmoothingEnabled = false;
    ctx.lineWidth = 0.2;
    const padding = ctx.lineWidth;
    const pixelX = x*(pixelSize);
    const pixelY = y*(pixelSize);

    ctx.fillStyle = color;
    ctx.fillRect(pixelX + padding, pixelY + padding , pixelSize - 2*padding, pixelSize - 2*padding);

//
};

window.getNumberOfPixels = (element) => {
    if(parseInt(element.value))
    return parseInt(element.value);
    else return 0;
};