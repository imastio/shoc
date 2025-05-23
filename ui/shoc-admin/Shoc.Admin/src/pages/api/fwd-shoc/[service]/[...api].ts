import { auth } from "@/addons/auth";
import { getJwtNode } from "@/addons/auth/actions";
import httpProxy from "http-proxy";
import { NextApiRequest, NextApiResponse, PageConfig } from "next";

export const config: PageConfig = {
  api: {
    externalResolver: true,
    bodyParser: false
  },
};


export default async function handle(req: NextApiRequest, res: NextApiResponse) {

  await auth(req, res);
  
  const jwt = await getJwtNode(req.headers);
  const proxy: httpProxy = httpProxy.createProxy();

  let apiRoot = process.env.SHOC_API_ROOT || '';
  if (apiRoot.endsWith('/')) {
    apiRoot = apiRoot.replace(/\/$/, "");
  }

  req.url = req.url?.replace('/api/fwd-shoc', '');

  const sse = req.headers["x-shoc-sse"];
  if(Array.isArray(sse) && sse[0] === 'yes' || sse === 'yes'){ 
    res.appendHeader('Content-Encoding', 'none')
  }
  
  proxy.web(req, res, {
    changeOrigin: true,
    target: apiRoot,
    secure: process.env.NODE_TLS_REJECT_UNAUTHORIZED !== '0',
    headers: {
      'Authorization': `Bearer ${jwt?.actualAccessToken || ''}`,
      'Cookie': ''
    }
  }, () => { });
}