/** @type {import('next').NextConfig} */
const nextConfig = {
    compiler: {
        reactStrictMode: true,
        styledComponents: true
    },
    output: "export",
}
const withSvgr = require('next-svgr');

module.exports = nextConfig
module.exports = withSvgr()