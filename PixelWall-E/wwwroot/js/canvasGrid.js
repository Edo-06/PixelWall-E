
window.drawGrid = (canvasId, pixelSize, color) => {
    const canvas = document.getElementById(canvasId);
    const ctx = canvas.getContext('2d');
    const width = canvas.width;
    const height = canvas.height;

    ctx.strokeStyle = color;
    ctx.lineWidth = 0.2;

    
    for (let x = 0.0; x <= width; x += pixelSize) {
        ctx.beginPath();
        ctx.moveTo(x, 0);
        ctx.lineTo(x, height);
        ctx.stroke();
    }

    
    for (let y = 0.0; y <= height; y += pixelSize) {
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

window.fillPixel = (canvasId, pixelSize, x, y, color) => {
    const canvas = document.getElementById(canvasId);
    const ctx = canvas.getContext('2d');
    const pixelX = (x-1)*pixelSize + pixelSize/2;
    const pixelY = (y-1)*pixelSize + pixelSize/2;

    ctx.fillStyle = color;
    ctx.fillRect(pixelX, pixelY, pixelSize, pixelSize);

    ctx.strokeStyle = "#000000";
    ctx.strokeReact(pixelX, pixelY, pixelSize);
};

window.getNumberOfPixels = (element) => {
    return element.value ? parseInt(element.value) : 0;
};