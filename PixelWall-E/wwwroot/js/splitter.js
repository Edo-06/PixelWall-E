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
                    console.error("Error: One or more splitter elements were not found.");
                    return;
                }

                console.log("Splitter elements found. Initializing splitter.");

                let isDragging = false;
                let startY; // Initial Y position of the pointer
                let initialTopPanelHeight; // Height of the top panel when drag starts

                const minPanelHeight = 50; // Minimum height in pixels for each panel
                const splitterHeight = splitterElement.offsetHeight; // Get current splitter height in px

                // Determine if we are in a mobile layout (columns) vs desktop (rows)
                // This will help in deciding how to calculate available height for splitting
                const isMobileLayout = window.innerWidth < 768;

                // Event handler for when the mouse button is pressed or touch starts on the splitter
                const onInteractionStart = (e) => {
                    isDragging = true;
                    // Prevent default browser behavior (e.g., scrolling, image dragging, pinch-zoom)
                    e.preventDefault(); 
                    
                    startY = (e.touches ? e.touches[0].clientY : e.clientY);
                    initialTopPanelHeight = topPanelElement.offsetHeight; 

                    document.body.style.userSelect = 'none'; // Prevent text selection during drag
                    document.body.style.cursor = 'ns-resize'; // Change cursor for dragging

                    // If on mobile, force the container to take full viewport height during drag
                    // This ensures the splitter has a stable reference to split across the screen.
                    if (isMobileLayout) {
                        splitContainerElement.style.height = `${window.innerHeight - splitContainerElement.getBoundingClientRect().top}px`;
                        // Recalculate initialTopPanelHeight because splitContainerElement's height might have changed.
                        initialTopPanelHeight = topPanelElement.offsetHeight; 
                    }
                };

                // Event handler for when the mouse moves or touch moves (while dragging)
                const onInteractionMove = (e) => {
                    if (!isDragging) return;

                    const currentY = (e.touches ? e.touches[0].clientY : e.clientY);
                    const deltaY = currentY - startY;

                    let newTopHeight = initialTopPanelHeight + deltaY;

                    let effectiveContainerHeight;
                    // On mobile, if we want right-bottom to fill the screen,
                    // the splitting context is the full viewport height below the .right panel's top.
                    if (isMobileLayout) {
                        // Calculate available height from the top of the .right panel to the bottom of the viewport
                        effectiveContainerHeight = window.innerHeight - splitContainerElement.getBoundingClientRect().top;
                        
                        // Adjust newTopHeight relative to the top of its parent, not just initialTopPanelHeight + deltaY
                        // This ensures the splitter aligns with currentY in the viewport
                        newTopHeight = currentY - topPanelElement.getBoundingClientRect().top + topPanelElement.offsetHeight - splitterElement.offsetHeight; // More dynamic approach
                        
                        // Recalculate initialTopPanelHeight based on current position relative to viewport
                        // This is a complex interaction. Let's simplify and rely on the fixed container height if possible.
                        // The primary goal is for `right-bottom` to fill the remaining space.
                        // If `splitContainerElement` is not `100vh` on mobile, its `offsetHeight` will be the height of `topPanelElement` + `splitter` + `bottomPanelElement`.
                        // When dragging, `newTopHeight` can become very small.
                        // The `newBottomHeight` needs to be `effectiveContainerHeight - newTopHeight - splitterHeight`.
                    } else {
                        effectiveContainerHeight = splitContainerElement.offsetHeight;
                    }

                    const maxTopHeight = effectiveContainerHeight - splitterHeight - minPanelHeight;

                    // Apply constraints
                    if (newTopHeight < minPanelHeight) {
                        newTopHeight = minPanelHeight;
                    } else if (newTopHeight > maxTopHeight) {
                        newTopHeight = maxTopHeight;
                    }
                    
                    // Calculate the new bottom height based on the new top height
                    const newBottomHeight = effectiveContainerHeight - newTopHeight - splitterHeight;

                    topPanelElement.style.height = `${newTopHeight}px`;
                    bottomPanelElement.style.height = `${newBottomHeight}px`; 
                };

                // Event handler for when the mouse button is released or touch ends
                const onInteractionEnd = () => {
                    isDragging = false;
                    document.body.style.userSelect = ''; // Reset user-select
                    document.body.style.cursor = '';     // Reset cursor

                    // On mobile, remove the fixed height set during drag to allow natural flow if needed.
                    // However, if the goal is for right-bottom to *stay* expanded, we might not remove this.
                    // For a consistent splitter experience, it's better if `splitContainerElement`
                    // maintains a height (e.g., 100vh)
                    if (isMobileLayout) {
                        // Re-evaluate if height: auto is desired after dragging.
                        // If the goal is for `right-bottom` to *remain* filling the screen,
                        // `splitContainerElement.style.height` might need to stay.
                        // Let's assume for now the JS will control the pixel height.
                        // Or, better, if .right is always 100vh on mobile, this is simpler.
                    }
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
                        let currentEffectiveContainerHeight;
                        if (isMobileLayout) {
                            currentEffectiveContainerHeight = window.innerHeight - splitContainerElement.getBoundingClientRect().top;
                        } else {
                            currentEffectiveContainerHeight = splitContainerElement.offsetHeight;
                        }
                        
                        const totalPanelSpace = currentEffectiveContainerHeight - splitterHeight;
                        if (totalPanelSpace <= 0) return;

                        const currentTopHeightPx = topPanelElement.offsetHeight;
                        const currentBottomHeightPx = bottomPanelElement.offsetHeight;

                        const currentTotalContentHeight = currentTopHeightPx + currentBottomHeightPx;
                        if (currentTotalContentHeight === 0) { // If initially empty, set default
                             topPanelElement.style.height = `${totalPanelSpace * 0.5}px`; // Default to 50%
                             bottomPanelElement.style.height = `${totalPanelSpace * 0.5}px`;
                             return;
                        }

                        const topRatio = currentTopHeightPx / currentTotalContentHeight;
                        const bottomRatio = currentBottomHeightPx / currentTotalContentHeight;

                        topPanelElement.style.height = `${topRatio * totalPanelSpace}px`;
                        bottomPanelElement.style.height = `${bottomRatio * totalPanelSpace}px`;
                    }
                };

                updatePanelHeightsOnResize(); // Call once on initialization
                window.addEventListener('resize', updatePanelHeightsOnResize);
            }
        };

        document.addEventListener('DOMContentLoaded', () => {
            const splitterElement = document.querySelector('.splitter');
            const rightTopElement = document.querySelector('.right-top');
            const rightBottomElement = document.querySelector('.right-bottom');
            const rightPanelElement = document.querySelector('.right'); // The container for splitting

            // Initialize the splitter only if all elements are found
            if (splitterElement && rightTopElement && rightBottomElement && rightPanelElement) {
                window.splitterJsInterop.initialize(splitterElement, rightTopElement, rightBottomElement, rightPanelElement);
            } else {
                console.warn("Could not initialize splitter: one or more required elements were not found.");
            }
        });