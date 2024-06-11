import { getJwt } from "@/addons/auth";
import axios from "axios";
import httpProxy from "http-proxy";
import { NextRequest, NextResponse } from "next/server";

async function handler(request: NextRequest, context: { params: any }){

  const jwt = await getJwt();
  const { params } = context;
  const { service, api }: { service: string, api: string[] } = params;
  const url = new URL(request.url)

  let apiRoot = process.env.SHOC_ADMIN_API_ROOT || '';
  
  if(!apiRoot.endsWith('')){
    apiRoot = `${apiRoot}/`
  }

  const newUrl = `${apiRoot}/${service}/${api.join('/')}${url.search}`;

  return NextResponse.json({newUrl})
  
}

export { handler as GET, handler as POST, handler as PUT, handler as PATCH, handler as DELETE };