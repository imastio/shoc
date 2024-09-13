import { NextResponse } from "next/server";

export function GET(request, context){
    try {
        return NextResponse.json([{status: 'ok'}])
    }
    catch(e){
        return NextResponse.json({err: JSON.stringify(e)}, { status: 400 })
    }
    
}