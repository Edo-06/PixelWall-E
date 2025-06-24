window.getNumberOfPixels = (element) => {
    if(element === undefined || element === null) return 0;
    if(parseInt(element.value))
    return parseInt(element.value);
    return 0;
};