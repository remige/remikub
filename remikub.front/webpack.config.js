const path = require("path");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const StyleLintPlugin = require("stylelint-webpack-plugin");
const HtmlWebpackPlugin = require("html-webpack-plugin");
const HtmlWebpackHarddiskPlugin = require("html-webpack-harddisk-plugin");
const uid = new Date().getTime();
const webpack = require("webpack");
const HappyPack = require("happypack");
const ForkTsCheckerWebpackPlugin = require("fork-ts-checker-webpack-plugin");

const isDev = !process.argv.includes("-p");

const rules = [
  {
    test: /\.tsx?$/,
    include: [path.resolve(__dirname, "./app")],

    use: [{
      loader: "babel-loader",
    }, {
      loader: "happypack/loader?id=ts",
    }],
  },
  {
    test: /\.(eot|svg|ttf|woff|woff2|otf|png|ico|jpg)$/,
    use: [{
      loader: "file-loader",
      options: {
        outputPath: "files/",
        name: "[name].[ext]",
      },
    }],
  },
  {
    test: /\.(sa|sc|c)ss$/,
    use: [
      MiniCssExtractPlugin.loader,
      "css-loader",
      "sass-loader",
    ],
  },
];

const plugins = [
  new HappyPack({
    id: "ts",
    threads: 3,
    loaders: [
      {
        path: "ts-loader",
        query: { happyPackMode: true },
      },
    ],
  }),
  new ForkTsCheckerWebpackPlugin({ tslint: isDev }),
  new MiniCssExtractPlugin({
    filename: `[name]/css/[name]-[hash].css`,
    chunkFilename: `[name]/css/[name]-[hash].css`,
    disable: false,
  }),
  new HtmlWebpackPlugin({
    template: "index.html.ejs",
    favicon: "app/img/favicon.ico",
    alwaysWriteToDisk: true,
    chunks: ["front"],
  }),
  new HtmlWebpackHarddiskPlugin({
    outputPath: isDev ? "../remikub/wwwroot" : "./dist",
  }),
  new webpack.IgnorePlugin(/^\.\/locale$/, /moment$/),
];

if (isDev) {
  plugins.push(
    new StyleLintPlugin({
      files: ["./app/**/*.scss"],
    }));
}

process.traceDeprecation = true;
module.exports = en => {
  return {
    mode: isDev ? "development" : "production",
    entry: {
      front: "./app/initializer.ts",
    },
    output: {
      path: path.join(__dirname, "dist/"),
      filename: `[name]/js/[name]-[hash].js`,
      publicPath: isDev ? `http://localhost:3000/` : "/",
    },
    resolve: {
      extensions: [".js", ".ts", ".tsx"],
      alias: {
        "./dist/cpexcel.js": "",
      },
    },
    plugins,
    module: {
      rules,
    },
    devtool: isDev ? "source-map" : "cheap-source-map",
  };
};
