FROM node:18-slim as base
ARG PORT=3000
WORKDIR /src
RUN corepack enable pnpm

# Build
FROM base as build

COPY --link package.json pnpm-lock.yaml ./
RUN pnpm install

ENV NODE_ENV=production
COPY --link . .
RUN pnpm run build

# Run
FROM base
ENV NODE_ENV=production
ENV PORT=$PORT
ENV NUXT_API_URL=$services__apiservice__https__0
COPY --from=build /src/.output /src/.output
CMD [ "node", ".output/server/index.mjs" ]
