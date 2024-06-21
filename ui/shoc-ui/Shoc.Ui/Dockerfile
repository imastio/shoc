FROM node:lts-alpine
WORKDIR /app

ENV NODE_ENV production
# ENV NEXT_TELEMETRY_DISABLED 1

RUN addgroup --system --gid 1001 nodejs
RUN adduser --system --uid 1001 nextjs

COPY ./public ./public

# Set the correct permission for prerender cache
RUN mkdir .next
RUN chown nextjs:nodejs .next

# Automatically leverage output traces to reduce image size
# https://nextjs.org/docs/advanced-features/output-file-tracing
COPY --chown=nextjs:nodejs /.next/standalone ./
COPY --chown=nextjs:nodejs /.next/static ./.next/static

USER nextjs

EXPOSE 11040

ENV PORT 11040
ENV HOSTNAME "0.0.0.0"

WORKDIR /tmp
RUN npm install --platform=linuxmusl --arch=x64 sharp
ENV NEXT_SHARP_PATH=/tmp/node_modules/sharp 

WORKDIR /app

# server.js is created by next build from the standalone output
# https://nextjs.org/docs/pages/api-reference/next-config-js/output
CMD ["node", "server.js"]