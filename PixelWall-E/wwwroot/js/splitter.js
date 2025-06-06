window.splitterJsInterop = {
    /**
     * Inicializa la funcionalidad de arrastre para un splitter vertical.
     * @param {HTMLElement} splitterElement 
     * @param {HTMLElement} topPanelElement 
     * @param {HTMLElement} bottomPanelElement 
     * @param {HTMLElement} splitContainerElement 
     */
    initialize: function (splitterElement, topPanelElement, bottomPanelElement, splitContainerElement) {
        if (!splitterElement || !topPanelElement || !bottomPanelElement || !splitContainerElement) {
            console.error("Error: Uno o más elementos del splitter no fueron encontrados.");
            return;
        }

        console.log("Splitter elements found. Initializing splitter.");

        let isDragging = false;
        let startY; 
        let initialTopPanelHeight; 

        const minPanelHeight = 50; 
        const splitterHeight = splitterElement.offsetHeight; 

        // Event handler para cuando se presiona el botón del ratón en el splitter
        const onMouseDown = (e) => {
            isDragging = true;
            startY = e.clientY;
            initialTopPanelHeight = topPanelElement.offsetHeight; // Obtiene la altura actual del panel superior

            document.body.style.userSelect = 'none';
            document.body.style.cursor = 'ns-resize';

            e.preventDefault(); // Previene el comportamiento de arrastre por defecto del navegador (ej. arrastrar imágenes)
        };

        // Event handler para cuando el ratón se mueve (mientras se está arrastrando)
        const onMouseMove = (e) => {
            if (!isDragging) return;

            const deltaY = e.clientY - startY; 
            let newTopHeight = initialTopPanelHeight + deltaY; 

            const containerHeight = splitContainerElement.offsetHeight; 

            const maxTopHeight = containerHeight - splitterHeight - minPanelHeight;

            if (newTopHeight < minPanelHeight) {
                newTopHeight = minPanelHeight;
            } else if (newTopHeight > maxTopHeight) {
                newTopHeight = maxTopHeight;
            }

            topPanelElement.style.height = `${newTopHeight}px`;
        };

        // Event handler para cuando se suelta el botón del ratón
        const onMouseUp = () => {
            isDragging = false; 

            document.body.style.userSelect = '';
            document.body.style.cursor = '';
        };

        splitterElement.addEventListener('mousedown', onMouseDown);

        document.addEventListener('mousemove', onMouseMove);
        document.addEventListener('mouseup', onMouseUp);
    }
};