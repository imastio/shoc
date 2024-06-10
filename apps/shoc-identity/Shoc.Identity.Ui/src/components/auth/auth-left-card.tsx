import { useIntl } from "react-intl"

export default function AuthLeftCard() {
  const intl = useIntl();

  return <div className="relative hidden h-full flex-col bg-muted p-10 text-white lg:flex dark:border-r">
    <div className="absolute inset-0 bg-zinc-900" />
    <div className="relative z-20 flex items-center text-lg font-medium">

      <svg
        xmlns="http://www.w3.org/2000/svg"
        viewBox="0 0 24 24"
        fill="none"
        stroke="currentColor"
        strokeWidth="2"
        strokeLinecap="round"
        strokeLinejoin="round"
        className="mr-2 h-6 w-6"
      >
        <path d="M15 6v12a3 3 0 1 0 3-3H6a3 3 0 1 0 3 3V6a3 3 0 1 0-3 3h12a3 3 0 1 0-3-3" />
      </svg>
      {intl.formatMessage({id: 'auth.platform'})}
    </div>
    <div className="relative z-20 mt-auto hidden">
      <blockquote className="space-y-2">
        <p className="text-lg">
          A cool quote in case if one day I will come up with it.
        </p>
        <footer className="text-sm">Me Myself</footer>
      </blockquote>
    </div>
  </div>
}