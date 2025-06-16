require(['vs/editor/editor.main'], function() {
    monaco.languages.register({
        id: 'pixelwalle-console', 
        aliases: ['PixelWallE Console', 'pixelwalle-console'],
        mimetypes: ['application/pixelwalle-console'],
    });

    monaco.languages.setMonarchTokensProvider('pixelwalle-console', {
        // defaultToken se usa si ninguna otra regla coincide.
        // Lo mantendremos en 'output.text' para que el texto normal tenga un color base.
        defaultToken: 'output.text', 

        tokenizer: {
            root: [
                // 1. Errores y Comandos (línea completa) - ¡Estas son las más específicas y deben ir PRIMERO!
                [/^Compiling Error:.*$/, 'error.compiling'],
                [/^Lexer Exception:.*$/, 'error.lexer'],
                [/^Runtime Error:.*$/, 'error.runtime'],

                [/run\b.*$/, 'status.run'],    // 'run' en amarillo
                [/help\b.*$/, 'console.command'], // 'help' en azul
                [/clean\b.*$/, 'console.command'],// 'clean' en azul

                // 2. Cadenas de texto entre comillas - Muy específicas, deben ir antes que los identificadores genéricos.
                [/".*?"/, 'output.string'], // Coincide la cadena completa, incluyendo comillas.

                // 3. Números - También bastante específicos.
                [/\d+/, 'output.number'], 
                
                // 4. Etiquetas/Nombres seguidos de dos puntos (ej: "Variable: valor") - Específico.
                [/[a-zA-Z_][a-zA-Z0-9_\-]*:\s*/, 'output.label'], 

                // 5. Comentarios (si esperas comentarios de línea en la consola, aunque no es común)
                [/\/\/.*$/, 'comment'], // Si hay comentarios de línea como `// Este es un comentario`

                // 6. Finalmente, cualquier otro carácter se clasifica como texto normal de salida.
                // Esta debe ser la ÚLTIMA regla en el array root.
                [/./, 'output.text'], 
            ],
        },
    });

    monaco.editor.defineTheme('pixelwalle-console-dark-theme', {
        base: 'vs-dark',
        inherit: true,
        rules: [
            
            { token: 'output.text', foreground: 'D4D4D4' },
            { token: 'output.number', foreground: 'B5CEA8' },
            { token: 'output.label', foreground: '9CDCFE' },
            { token: 'output.string', foreground: 'D69D85' },
            { token: 'comment', foreground: '6A9955' },
        ],
        colors: {}
    });

    monaco.editor.defineTheme('pixelwalle-console-light-theme', {
        base: 'pixelwalle-light-theme',
        inherit: true,
        rules: [
            
            { token: 'output.text', foreground: '000000' },
            { token: 'output.number', foreground: '098677' },
            { token: 'output.label', foreground: '000080' },
            { token: 'output.string', foreground: 'A31515' },
            { token: 'comment', foreground: '008000' },
        ],
        colors: {}
    });

    monaco.editor.defineTheme('pixelwalle-console-high-contrast-black-theme', {
        base: 'hc-black',
        inherit: true,
        rules: [
            
            { token: 'output.text', foreground: 'D4D4D4' },
            { token: 'output.number', foreground: 'B5CEA8' },
            { token: 'output.label', foreground: '9CDCFE' },
            { token: 'output.string', foreground: 'D69D85' },
            { token: 'comment', foreground: '6A9955' },
        ],
        colors: {}
    });
});