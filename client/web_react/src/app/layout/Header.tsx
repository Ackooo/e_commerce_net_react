import { Logout, Person, Settings, ShoppingCart } from "@mui/icons-material";
import { AppBar, Badge, Box, IconButton, Switch, TextField, Toolbar, Tooltip, Typography } from "@mui/material";
import { Link, useNavigate } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "../store/configureStore";
import DropdownMenu, { MenuOption } from "../components/AppDropdownMenu";
import { signOut } from "../../features/account/accountSlice";
import { clearBasket } from "../../features/basket/basketSlice";
import { User } from "../models/user";

import agent from "../api/agent";
import Flag from 'react-world-flags';

import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import ShoppingBasketIcon from '@mui/icons-material/ShoppingBasket';
import HomeIcon from '@mui/icons-material/Home';
import LanguageIcon from '@mui/icons-material/Language';
import ArticleIcon from '@mui/icons-material/Article';
import WarehouseIcon from '@mui/icons-material/Warehouse';
import AssessmentIcon from '@mui/icons-material/Assessment';
import SettingsIcon from '@mui/icons-material/Settings';
import InventoryIcon from '@mui/icons-material/Inventory';
import GroupIcon from '@mui/icons-material/Group';
import GroupAddIcon from '@mui/icons-material/GroupAdd';
import InfoIcon from '@mui/icons-material/Info';
import VpnKeyIcon from '@mui/icons-material/VpnKey';
import TranslateIcon from '@mui/icons-material/Translate';
import DarkModeIcon from '@mui/icons-material/DarkMode';
import LoginIcon from '@mui/icons-material/Login';
import ArchiveIcon from '@mui/icons-material/Archive';
import LogoDevIcon from '@mui/icons-material/LogoDev';
import BugReportIcon from '@mui/icons-material/BugReport';
import AssistantIcon from '@mui/icons-material/Assistant';
import ChatIcon from '@mui/icons-material/Chat';
import CreditCardIcon from '@mui/icons-material/CreditCard';
import MiscellaneousServicesIcon from '@mui/icons-material/MiscellaneousServices';
import FlagIcon from '@mui/icons-material/Flag';



const navStyles = {
    color: 'inherit',
    textDecoration: 'none',
    // typography: 'h6',
    cursor: 'pointer',
    '&:hover': {
        color: 'grey.500'
    },
    '&.active': {
        color: 'text.secondary'
    },
    minWidth: 60,
}

const secondRowIconStyles = {
    cursor: 'pointer',
    padding: 1.5,
    borderRadius: 2,
    border: '1px solid #ddd',
    backgroundColor: '',
    transition: 'all 0.3s ease',
    '&:hover': {
        backgroundColor: 'primary',
        color: 'grey.500',
        boxShadow: '0px 4px 10px rgba(0, 0, 0, 0.15)',
        transform: 'scale(1.1)',
    },
}

interface Props {
    darkMode: boolean;
    handleThemeChange: () => void;
}

const createAccountOptions = (user: User | null, darkMode: boolean, handleThemeChange: () => void): MenuOption[] => {
    const darkModeSwitch = (
        <Box display="flex" alignItems="center" justifyContent="space-between" width="100%">
            <Typography variant="body2">Dark Mode</Typography>
            <Switch size="small" checked={darkMode} onChange={handleThemeChange} />
        </Box>
    );

    if (!user) {
        return [
            { label: 'Login', icon: <LoginIcon />, route: '/login' },
            { label: 'Register', icon: <LoginIcon />, route: '/register' },
            { label: darkModeSwitch, icon: <DarkModeIcon />, dividerBefore: true, clickable: true },

        ];
    }

    return [
        { label: 'Profile', icon: <Person />, route: '/profile' },
        { label: 'Settings', icon: <Settings />, dividerBefore: true, route: '/settings' },
        { label: 'My Orders', icon: <ShoppingBasketIcon />, route: '/orders' },
        { label: 'My credit cards', icon: <CreditCardIcon />, route: '/creditCards' },
        { label: darkModeSwitch, icon: <DarkModeIcon />, clickable: true },

    ];
};

const createCultureOptions = (): MenuOption[] => {
    return [
        { label: 'English', icon: <Flag code="gb" style={{ width: 24, height: 16, marginRight: 8 }} />, onClick: () => { agent.Account.setCulture('en-US') } },
        { label: 'Srpski', icon: <Flag code="srb" style={{ width: 24, height: 16, marginRight: 8 }} />, onClick: () => { agent.Account.setCulture('sr') } },
    ];
}

const createWarehouseOptions = (): MenuOption[] => {
    return [
        { label: 'Manage warehouses', icon: <SettingsIcon />, route: '/warehouses' },
        { label: 'Reports', icon: <AssessmentIcon />, route: '/reports' },

    ];
}

const createUsersOptions = (): MenuOption[] => {
    return [
        { label: 'Manage users', icon: <GroupAddIcon />, route: '/users' },
        { label: 'Statistics', icon: <AssessmentIcon />, route: '/statistics' },

    ];
}

const createConfigurationOptions = (): MenuOption[] => {
    return [
        { label: 'Api Key config', icon: <VpnKeyIcon />, route: '/apiConfig' },
        { label: 'Api Localization update', icon: <TranslateIcon />, route: '/apiLocalization' },
        { label: 'Api services management', icon: <MiscellaneousServicesIcon />, route: '/apiServices' },
        { label: 'Feature flags', icon: <FlagIcon />, route: '/flags' },
        { label: 'Api Http Test', icon: <LogoDevIcon />, route: '/about' },
    ];
}

const createLogsOptions = (): MenuOption[] => {
    return [
        { label: 'Audit logs', icon: <LoginIcon />, route: '/' },
        { label: 'Incident Logs', icon: <BugReportIcon />, route: '/' },
        { label: 'App logs', icon: <InfoIcon />, route: '/about' },

    ];
}


export default function Header({ darkMode, handleThemeChange }: Props) {
    //const {basket} = useStoreContext(); move to redux below
    const dispatch = useAppDispatch();
    const navigate = useNavigate();
    const { basket } = useAppSelector(state => state.basket);
    const { user } = useAppSelector(state => state.account);
    const itemCount = basket?.items.reduce((sum, item) => sum + item.quantity, 0);

    const accountOptions = createAccountOptions(user, darkMode, handleThemeChange);

    if (user) {
        accountOptions.push({ label: 'Logout', icon: <Logout />, onClick: () => { dispatch(signOut()); dispatch(clearBasket()); } });
    }

    const getUserMenu = () => {
        if (!user)
            return <DropdownMenu customAvatar={<AccountCircleIcon fontSize="large" />} tooltip="Account Settings" options={accountOptions} />;

        var firstLetter = user.email.charAt(0).toUpperCase();
        return <DropdownMenu avatarLabel={firstLetter} tooltip="Account Settings" options={accountOptions} />
    }

    return (
        <AppBar position="static" >
            <Toolbar sx={{ display: 'flex', flexDirection: 'column', justifyContent: 'space-between', alignItems: 'stretch', padding: 2, gap: 1.5 }}>

                {/* Top Row: Theme Switch & Language & User Menu & Basket*/}
                <Box display="flex" justifyContent="space-between" alignItems="center" sx={{ minHeight: 32, paddingY: 0.5, }}>

                    <Box display='flex' alignItems='center' >
                        <DropdownMenu customAvatar={<LanguageIcon fontSize="medium" />} tooltip="Change language" options={createCultureOptions()} />
                    </Box>

                    <Box display='flex' alignItems='center'>
                        {user && //(user.roles?.includes('Admin') || user.roles?.includes('SuperAdmin')) &&
                            <DropdownMenu customAvatar={<WarehouseIcon fontSize="large" />} tooltip="Warehouse" options={createWarehouseOptions()} />
                        }
                        {user && //(user.roles?.includes('Admin') || user.roles?.includes('SuperAdmin')) &&
                            <DropdownMenu customAvatar={<GroupIcon fontSize="large" />} tooltip="Manage users" options={createUsersOptions()} />
                        }
                        {user && //(user.roles?.includes('Admin') || user.roles?.includes('SuperAdmin')) &&
                            <DropdownMenu customAvatar={<SettingsIcon fontSize="large" />} tooltip="Configuration" options={createConfigurationOptions()} />
                        }
                        {user && //(user.roles?.includes('Admin') || user.roles?.includes('SuperAdmin')) &&
                            <DropdownMenu customAvatar={<ArchiveIcon fontSize="large" />} tooltip="Logs" options={createLogsOptions()} />
                        }
                    </Box>

                    <Box display='flex' alignItems='center'>
                        <Tooltip title="Chat" >
                            <ChatIcon onClick={() => navigate('/chat')}></ChatIcon>
                        </Tooltip>
                        <Tooltip title="Basket" >
                            <IconButton component={Link} to='/basket' size="medium" edge='start' color="inherit" sx={{ mr: 0, ml: 1 }}>
                                <Badge badgeContent={itemCount} color="secondary">
                                    <ShoppingCart />
                                </Badge>
                            </IconButton>
                        </Tooltip>
                        {getUserMenu()}
                    </Box>
                </Box>

                {/* Middle Row: Navigation Icons */}
                <Box display='flex' justifyContent="center" alignItems="center" gap={2} >
                    <Tooltip title="Home" >
                        <Box display="flex" alignItems="center" sx={{ ...secondRowIconStyles, marginRight: 2, marginLeft: -7 }} onClick={() => navigate('/')}  >
                            <HomeIcon fontSize="large" />
                            <Typography variant="h6" sx={{ ml: 1, mr: 1 }}>E_commerce_net_react</Typography>
                        </Box>
                    </Tooltip>

                    <Tooltip title="Catalog" >
                        <Box display="flex" alignItems="center" sx={{ ...secondRowIconStyles, marginRight: 2, marginLeft: 0, px: 1.5, py: 1, }} onClick={() => navigate('/catalog')}  >
                            <ArticleIcon fontSize="medium" sx={navStyles} ></ArticleIcon>
                            <Typography variant="body1" sx={{ ml: -1, mr: 2 }}>Catalog</Typography>
                        </Box>
                    </Tooltip>

                    {user &&// (user.roles?.includes('Vendor') || user.roles?.includes('Admin') || user.roles?.includes('SuperAdmin')) &&
                        <Tooltip title="Inventory" >
                            <Box display="flex" alignItems="center" sx={{ ...secondRowIconStyles, marginRight: 2, marginLeft: 0, px: 1.5, py: 1, }} onClick={() => navigate('/inventory')}  >
                                <InventoryIcon fontSize="medium" sx={navStyles} ></InventoryIcon>
                                <Typography variant="body1" sx={{ ml: -1, mr: 2 }}>Inventory</Typography>
                            </Box>
                        </Tooltip>
                    }

<Tooltip title="Assistant" >
                        <Box display="flex" alignItems="center" sx={{ ...secondRowIconStyles, marginRight: 2, marginLeft: 0, px: 1.5, py: 1, }} onClick={() => navigate('/catalog')}  >
                            <AssistantIcon fontSize="medium" sx={navStyles} ></AssistantIcon>
                            <Typography variant="body1" sx={{ ml: -1, mr: 2 }}>Assistant</Typography>
                        </Box>
                    </Tooltip>

                </Box>
                {/* Bottom Row: Global search */}
            </Toolbar>
        </AppBar>
    )
}