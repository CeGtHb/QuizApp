window.mathJaxInterop = {
    async render(element, latex) {
        await MathJax.startup.promise;

        const formula = await MathJax.tex2svgPromise(latex, {
            display: true
        });

        if (!document.getElementById('MJX-SVG-styles')) {
            document.head.appendChild(MathJax.svgStylesheet());
        }

        element.replaceChildren(formula);
    }
};