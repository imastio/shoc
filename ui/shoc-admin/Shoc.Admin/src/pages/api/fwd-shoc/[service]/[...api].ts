import { auth } from "@/addons/auth";
import { getJwtNode } from "@/addons/auth/jwt-node";
import httpProxy from "http-proxy";
import { NextApiRequest, NextApiResponse } from "next";

export const config = {
  api: {
    externalResolver: true,
    bodyParser: false,
  },
};


export default async function handle(req: NextApiRequest, res: NextApiResponse) {

  await auth(req, res);
  const jwt = await getJwtNode(req.headers);
  const proxy: httpProxy = httpProxy.createProxy();

  let apiRoot = process.env.SHOC_ADMIN_API_ROOT || '';
  if (apiRoot.endsWith('/')) {
    apiRoot = apiRoot.replace(/\/$/, "");
  }

  req.url = req.url?.replace('/api/fwd-shoc', '');

  proxy.web(req, res, {
    changeOrigin: true,
    target: apiRoot,
    secure: process.env.NODE_TLS_REJECT_UNAUTHORIZED !== '0',
    headers: {
      'Authorization': `Bearer ${jwt?.access_token || ''}`,
      'Cookie': ''
    }
  }, () => { });
}