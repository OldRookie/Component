({
    baseUrl: "..",
    mainConfigFile: "../config.js",
    dir: "../dist",
    optimize: "uglify2",
    inlineText: true,
    removeCombined: true,
    skipDirOptimize: true,
    modules: [
        {
            name: 'main',
           // include: ["..."]
        }
    ]
})