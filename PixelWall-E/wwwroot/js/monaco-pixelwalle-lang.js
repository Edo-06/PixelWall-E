// monaco-pixelwalle-lang.js
// This code expects 'monaco' to be available before running.
// Ensure this script is loaded after the main Monaco editor script.

require(['vs/editor/editor.main'], function() {
    monaco.languages.register({ 
        id: 'pixelwalle',
        extensions: ['.pw'],
        aliases: ['PixelWallE', 'pixelwalle'],
        mimetypes: ['application/pixelwalle'],
    });

    monaco.languages.setMonarchTokensProvider('pixelwalle', {
        defaultToken: 'invalid', 

        keywords: [
            'Spawn',
            'MoveTo',   
            'Color',       
            'Size',        
            'DrawLine',    
            'DrawCircle',  
            'DrawRectangle',
            'Fill',       
            'GoTo'         
        ],

        functions: [
            'GetActualX',  
            'GetActualY',    
            'GetCanvasSize', 
            'GetColorCount', 
            'IsBrushColor',  
            'IsBrushSize',   
            'IsCanvasColor'  
        ],

        operators: [
            '+', '-', '*', '/', '**', '%', 
            '==', '>=', '<=', '>', '<',   
            '&&', '||',                   
            '<-'                      
        ],

        brackets: [
            { open: '(', close: ')', token: 'delimiter.parenthesis' },
            { open: '[', close: ']', token: 'delimiter.square' }
        ],

        tokenizer: {
            root: [
                // Identifiers: This rule attempts to match keywords, functions, variables, and labels.
                [/[a-zA-Z_][a-zA-Z0-9_\-]*/, {
                    cases: {
                        '@keywords': 'keyword',
                        '@functions': 'function',
                        '@default': 'identifier'
                    }
                }],

                // Whitespace
                { include: '@whitespace' },

                [/\/\/.*$/, 'comment'],

                [/"([^"\\]|\\.)*$/, 'string.invalid'],
                [/"/, { token: 'string.quote', bracket: '@open', next: '@string' }],

                [/\d+/, 'number'],
                
                [/[()]/, 'delimiter.parenthesis'],
                [/[\[\]]/, 'delimiter.square'],
                [/,/, 'delimiter.comma'],


                [/\*\*/, 'operator.arithmetic'], 
                [/[\+\-\*\/%]/, 'operator.arithmetic'],
                [/(==|>=|<=|>|<)/, 'operator.comparison'],
                [/(&&|\|\|)/, 'operator.boolean'],
                [/<-/, 'operator.assignment'],
            ],

            whitespace: [
                [/[ \t\r\n]+/, 'white']
            ],

            string: [
                [/[^"\\]+/, 'string'],
                [/\\./, 'string.escape'],
                [/"/, { token: 'string.quote', bracket: '@close', next: '@pop' }]
            ]
        },
    });

    // Define a custom theme for the Pixel Wall-E language
    monaco.editor.defineTheme('pixelwalle-dark-theme', {
        base: 'vs-dark', // Base theme (like Dark+ in VS Code)
        inherit: true,   // Inherit rules from the base theme
        rules: [
            { token: 'keyword', foreground: '569CD6' },              // Blue (e.g., Spawn, Color)
            { token: 'function', foreground: 'F0E68C' },          // Alternate more vibrant yellow for functions
            { token: 'identifier', foreground: '9CDCFE' },           // Light Blue (variables, labels)
            { token: 'comment', foreground: '6A9955' },              // Green
            { token: 'string', foreground: 'D69D85' },               // Orange/Brown (e.g., "Red")
            { token: 'string.quote', foreground: 'D69D85' },         // Quotes for strings
            { token: 'number', foreground: 'B5CEA8' },               // Light Green
            { token: 'operator.arithmetic', foreground: 'D4D4D4' },  // Light Gray/White
            { token: 'operator.comparison', foreground: 'D4D4D4' },
            { token: 'operator.boolean', foreground: '569CD6' },     // Blue, similar to keywords for &&, ||
            { token: 'operator.assignment', foreground: 'D4D4D4' },  // For <-
            { token: 'delimiter.parenthesis', foreground: 'C586C0' },// For () - Purple
            { token: 'delimiter.square', foreground: '667788' },     // For []
            { token: 'delimiter.comma', foreground: 'FFFFFF' },      // For , - White
            { token: 'invalid', foreground: 'CD3131' }         
        ],
        colors: {
        }
    });

    monaco.editor.defineTheme('pixelwalle-light-theme', {
    base: 'vs', // Base theme (like VS Code's Light+)
    inherit: true,
    rules: [
        { token: 'keyword', foreground: '0000FF' }, 
        { token: 'function', foreground: 'CC7832' }, // Un morado distintivo para funciones
        { token: 'identifier', foreground: '000080' },
        { token: 'comment', foreground: '008000' }, // Verde oscuro para comentarios, legible pero discreto
        { token: 'string', foreground: 'A31515' }, // Rojo oscuro para cadenas de texto
        { token: 'string.quote', foreground: 'A31515' }, // También las comillas
        { token: 'number', foreground: '098677' }, // Un verde azulado oscuro para números
        { token: 'operator.arithmetic', foreground: '000000' }, // Negro
        { token: 'operator.comparison', foreground: '000000' }, // Negro
        { token: 'operator.assignment', foreground: '000000' }, // Negro (para '->', etc.)
        { token: 'operator.boolean', foreground: '0000FF' }, // Azul, similar a las palabras clave
        { token: 'delimiter.parenthesis', foreground: '800080' }, // Morado para paréntesis
        { token: 'delimiter.square', foreground: '000000' },     // Negro para corchetes
        { token: 'delimiter.comma', foreground: '000000' },       // Negro para comas

        { token: 'invalid', foreground: 'FF0000' } // Rojo brillante para indicar errores 
    ],
        colors: {
        }
    });
    monaco.editor.defineTheme('pixelwalle-high-contrast-black-theme', {
    base: 'hc-black', // Base theme for high contrast black
    inherit: true,
    rules: [
        { token: 'keyword', foreground: '569CD6' },              // Blue (e.g., Spawn, Color)
        { token: 'function', foreground: 'FFFF00' },          // Alternate more vibrant yellow for functions
        { token: 'identifier', foreground: '9CDCFE' },           // Light Blue (variables, labels)
        { token: 'comment', foreground: '6A9955' },              // Green
        { token: 'string', foreground: 'D69D85' },               // Orange/Brown (e.g., "Red")
        { token: 'string.quote', foreground: 'D69D85' },         // Quotes for strings
        { token: 'number', foreground: 'B5CEA8' },               // Light Green
        { token: 'operator.arithmetic', foreground: 'D4D4D4' },  // Light Gray/White
        { token: 'operator.comparison', foreground: 'D4D4D4' },
        { token: 'operator.boolean', foreground: '569CD6' },     // Blue, similar to keywords for &&, ||
        { token: 'operator.assignment', foreground: 'D4D4D4' },  // For <-
        { token: 'delimiter.parenthesis', foreground: 'C586C0' },// For () - Purple
        { token: 'delimiter.square', foreground: 'FAFAD2' },     // For []
        { token: 'delimiter.comma', foreground: 'FFFFFF' },      // For , - White
        { token: 'invalid', foreground: 'CD3131' }   
    ],
    colors: {
        }
    });
});
