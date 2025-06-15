window.splitterJsInterop = {
    /**
     * Initializes drag functionality for a vertical splitter.
     * @param {HTMLElement} splitterElement
     * @param {HTMLElement} topPanelElement
     * @param {HTMLElement} bottomPanelElement
     * @param {HTMLElement} splitContainerElement
     */
    initialize: function (splitterElement, topPanelElement, bottomPanelElement, splitContainerElement) {
        if (!splitterElement || !topPanelElement || !bottomPanelElement || !splitContainerElement) {
            console.error("Error: One or more splitter elements were not found when JS was invoked from Blazor.");
            return;
        }

        console.log("Splitter elements found. Initializing splitter.");

        let isDragging = false;
        let startY; // Initial Y position of the pointer
        let initialTopPanelHeight; // Height of the top panel when drag starts
        let initialContainerHeight; // Height of the splitContainerElement when drag starts

        // Define minimum heights for panels.
        // MIN_TOP_PANEL_HEIGHT = 0 allows the top panel to effectively collapse.
        const MIN_TOP_PANEL_HEIGHT = 0; 
        // MIN_BOTTOM_PANEL_HEIGHT = 0 allows the bottom panel to effectively disappear.
        const MIN_BOTTOM_PANEL_HEIGHT = 0; 

        const splitterHeight = splitterElement.offsetHeight; // Get current splitter height in px

        // Determine if we are in a mobile layout (columns) vs desktop (rows)
        const isMobileLayout = window.innerWidth < 768;

        // Event handler for when the mouse button is pressed or touch starts on the splitter
        const onInteractionStart = (e) => {

            isDragging = true;
            // Prevent default browser behavior (e.g., scrolling, image dragging, pinch-zoom)
            e.preventDefault(); 
            
            startY = (e.touches ? e.touches[0].clientY : e.clientY);
            
            // IMPORTANT: If on mobile, set the container height *before* capturing initialTopPanelHeight
            // This ensures initialTopPanelHeight is relative to the new fixed container height.
            if (isMobileLayout) {
                splitContainerElement.style.height = `${window.innerHeight - splitContainerElement.getBoundingClientRect().top}px`;
            }
            
            initialTopPanelHeight = topPanelElement.offsetHeight; // Capture after container is set
            initialContainerHeight = splitContainerElement.offsetHeight; // Always capture after container is potentially set

            document.body.style.userSelect = 'none'; // Prevent text selection during drag
            document.body.style.cursor = 'ns-resize'; // Change cursor for dragging
        };

        // Event handler for when the mouse moves or touch moves (while dragging)
        const onInteractionMove = (e) => {
            if (!isDragging) return;

            const currentY = (e.touches ? e.touches[0].clientY : e.clientY);
            const deltaY = currentY - startY; // Delta from the start Y position

            let newTopHeight = initialTopPanelHeight + deltaY; // Calculate based on initial height and delta

            // Use the captured initial container height as the stable reference for splitting
            const effectiveContainerHeight = initialContainerHeight; 

            const maxTopHeight = effectiveContainerHeight - splitterHeight - MIN_BOTTOM_PANEL_HEIGHT;

            // Apply constraints
            if (newTopHeight < MIN_TOP_PANEL_HEIGHT) { // Use specific min for top panel
                newTopHeight = MIN_TOP_PANEL_HEIGHT;
            } else if (newTopHeight > maxTopHeight) {
                newTopHeight = maxTopHeight;
            }
            
            // Calculate the new bottom height based on the new top height and effective container height
            const newBottomHeight = effectiveContainerHeight - newTopHeight - splitterHeight;

            topPanelElement.style.height = `${newTopHeight}px`;
            bottomPanelElement.style.height = `${newBottomHeight}px`; 
        };

        // Event handler for when the mouse button is released or touch ends
        const onInteractionEnd = () => {
            isDragging = false;
            document.body.style.userSelect = ''; // Reset user-select
            document.body.style.cursor = '';     // Reset cursor
        };

        // --- Event Listeners ---
        splitterElement.addEventListener('mousedown', onInteractionStart);
        document.addEventListener('mousemove', onInteractionMove);
        document.addEventListener('mouseup', onInteractionEnd);

        splitterElement.addEventListener('touchstart', onInteractionStart, { passive: false });
        document.addEventListener('touchmove', onInteractionMove, { passive: false });
        document.addEventListener('touchend', onInteractionEnd);

        // Initial setup or re-calculation on resize
        const updatePanelHeightsOnResize = () => {
            if (!isDragging) {
                const currentTopHeightPx = topPanelElement.offsetHeight;
                const currentBottomHeightPx = bottomPanelElement.offsetHeight;
                const currentTotalContentHeight = currentTopHeightPx + currentBottomHeightPx;
                
                // Determine the total space for panels based on layout
                let totalPanelSpace;
                if (window.innerWidth < 768) { // Mobile layout
                    // Calculate total available height from top of split container to bottom of viewport
                    totalPanelSpace = window.innerHeight - splitContainerElement.getBoundingClientRect().top - splitterHeight;
                    // Ensure the split container itself fills this space for consistent splitting
                    splitContainerElement.style.height = `${window.innerHeight - splitContainerElement.getBoundingClientRect().top}px`;
                } else { // Desktop layout
                    totalPanelSpace = splitContainerElement.offsetHeight - splitterHeight;
                    splitContainerElement.style.height = '100vh'; // Ensure the container has 100vh on desktop
                }
               
                if (totalPanelSpace <= 0) return;

                if (currentTotalContentHeight === 0) { // If initially empty, set default split
                        topPanelElement.style.height = `${totalPanelSpace * 0.5}px`; // Default to 50%
                        bottomPanelElement.style.height = `${totalPanelSpace * 0.5}px`;
                        return;
                }

                // Maintain the current ratio when resizing
                const topRatio = currentTopHeightPx / currentTotalContentHeight;
                // Safety check for division by zero or invalid ratios
                if (isNaN(topRatio) || !isFinite(topRatio)) {
                    topPanelElement.style.height = `${totalPanelSpace * 0.5}px`;
                    bottomPanelElement.style.height = `${totalPanelSpace * 0.5}px`;
                    return;
                }

                let newTopHeight = topRatio * totalPanelSpace;
                let newBottomHeight = (1 - topRatio) * totalPanelSpace;

                // Re-apply min/max constraints after ratio calculation for consistency on resize
                if (newTopHeight < MIN_TOP_PANEL_HEIGHT) {
                    newTopHeight = MIN_TOP_PANEL_HEIGHT;
                    newBottomHeight = totalPanelSpace - newTopHeight;
                } else if (newBottomHeight < MIN_BOTTOM_PANEL_HEIGHT) { // Use specific min for bottom panel
                    newBottomHeight = MIN_BOTTOM_PANEL_HEIGHT;
                    newTopHeight = totalPanelSpace - newBottomHeight;
                }


                topPanelElement.style.height = `${newTopHeight}px`;
                bottomPanelElement.style.height = `${newBottomHeight}px`; 
            }
        };

        updatePanelHeightsOnResize(); // Call once on initialization
        window.addEventListener('resize', updatePanelHeightsOnResize);
    }
};
