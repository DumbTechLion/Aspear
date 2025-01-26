import { joinURL } from "ufo";

export default defineEventHandler(async (event) => {
  const session = await getUserSession(event);
  const proxyUrl = process.env.NUXT_API_URL as string;
  const path = event.path.replace(/^\/api\//, "");
  const target = joinURL(proxyUrl, path);

  console.log(target)

  return proxyRequest(event, target, {
    headers: {
      Authorization: `Bearer ${session?.tokens.accessToken}`,
    },
  });
});
