window.getNumberOfPixels = (element) => {
    if(parseInt(element.value))
    return parseInt(element.value);
    else return 0;
};