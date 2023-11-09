const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:58819';

const PROXY_CONFIG = [
  {
    context: [
      "/userauthentication",
      "/home",
      "/userlogin",
      "/userregister",
      "/userauth",
      "/mainpage",
      "/getpage",
      "/recipepage",
      "/getrecipe",
      "/admindata",
      "/getnumberoftags",
      "/gettags",
      "/createnewtag",
      "/edittag",

   ],
    proxyTimeout: 10000,
    target: target,
    secure: false,
    headers: {
      Connection: 'Keep-Alive'
    }
  }
]

module.exports = PROXY_CONFIG;
