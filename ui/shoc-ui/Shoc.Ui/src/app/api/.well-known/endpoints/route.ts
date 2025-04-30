import { NextResponse } from 'next/server';

export async function GET() {

  return NextResponse.json({
    api: process.env.SHOC_AUTH_ISSUER?.endsWith('/') ? process.env.SHOC_AUTH_ISSUER : `${process.env.SHOC_AUTH_ISSUER}/`,
    identity: process.env.SHOC_API_ROOT?.endsWith('/') ? process.env.SHOC_API_ROOT : `${process.env.SHOC_API_ROOT}/`
  });

}
