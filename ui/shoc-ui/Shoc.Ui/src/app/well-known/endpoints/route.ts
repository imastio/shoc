import { NextResponse } from 'next/server';
import { getIssuer } from '@/addons/auth/config';
import { shocApiConfig } from '@/clients/api-config';

export async function GET() {

  const data = {
    idp: getIssuer(),
    api: shocApiConfig().root
  };

  return NextResponse.json(data);
}
