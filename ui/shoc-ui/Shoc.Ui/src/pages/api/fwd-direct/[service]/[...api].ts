import { auth } from "@/addons/auth";
import { getJwtNode } from "@/addons/auth/actions";
import httpProxy from "http-proxy";
import { NextApiRequest, NextApiResponse } from "next";

export const config = {
  api: {
    externalResolver: true,
    bodyParser: false,
  },
};

export default async function handle(req: NextApiRequest, res: NextApiResponse) {

  const proxy: httpProxy = httpProxy.createProxy();

  let apiRoot = process.env.SHOC_UI_API_ROOT || '';
  if (apiRoot.endsWith('/')) {
    apiRoot = apiRoot.replace(/\/$/, "");
  }

  req.url = req.url?.replace('/api/fwd-direct', '');

  proxy.web(req, res, {
    changeOrigin: true,
    target: apiRoot,
    secure: process.env.NODE_TLS_REJECT_UNAUTHORIZED !== '0',
  }, () => { });
}