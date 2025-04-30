import { NextResponse } from 'next/server';
import { getIssuer } from '@/addons/auth/config';
import { shocApiConfig } from '@/clients/api-config';

export async function GET() {

  const idp = getIssuer();
  const api = shocApiConfig().root

  const data = {
    idp: idp.endsWith('/') ? idp.substring(0, idp.length - 1) : idp,
    api: api.endsWith('/') ? api.substring(0, idp.length - 1) : api
  };

  return NextResponse.json(data);
}
