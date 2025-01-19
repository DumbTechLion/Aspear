import { useApi, type OpenFetchClient } from "#imports";
import type { paths } from "#open-fetch-schemas/api";
type Fetch = OpenFetchClient<paths>;

export default function useAuthApiFetch() {
  const { $api, $toast } = useNuxtApp();

  const $authApi = ((...args: Parameters<Fetch>) => {
    const path = toValue(args[0]);
    const opt = args[1] ?? {
      headers: [],
    };
    opt.onResponseError = (error) => {
      $toast.error("Something wrong happened...", {
        timeout: 5000,
      });
    };
    return $api(path, opt);
  }) as Fetch;

  return { $authApi };
}
