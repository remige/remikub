var webpack = require("webpack");
var WebpackDevServer = require("webpack-dev-server");
var config = require("./webpack.config");

var conf = config({ chunk: "front-office" });
new WebpackDevServer(webpack(conf), {
    publicPath: conf.output.publicPath,
    inline: false,
    hot: false,
    headers: {
        "Access-Control-Allow-Origin": "*",
    },
    stats: {
        all: false,
        errors: true,
        warning: true,
    },
}).listen(3000, "localhost", function(err, result) {
    if (err) {
        // tslint:disable-next-line:no-console
        console.log(err);
    }
    // tslint:disable-next-line:no-console
    console.log("Listening at localhost:3000");
});
