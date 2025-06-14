window.downloadFile = (name, code) => {
    // Crear Blob y enlace temporal
    const blob = new Blob([code], { type: 'text/plain' });
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    
    // Configurar enlace
    a.href = url;
    a.download = name;
    a.style.display = 'none';
    
    // Disparar clic
    document.body.appendChild(a);
    a.click();
    
    // Limpiar
    window.URL.revokeObjectURL(url);
    document.body.removeChild(a);
};

window.focusInput = (element) => {
    element.focus();
};

window.getFileName = (element) => {
    return element.value;
};
window.saveAsFile = function (filename, dataUrl) {
    const link = document.createElement('a');
    link.href = dataUrl; 
    link.download = filename; 
    document.body.appendChild(link); 
    link.click(); 
    document.body.removeChild(link); 
};